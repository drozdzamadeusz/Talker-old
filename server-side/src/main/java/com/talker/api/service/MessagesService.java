package com.talker.api.service;

import com.talker.api.dao.ContactsDao;
import com.talker.api.dto.requests.ContactRequest;
import com.talker.api.entity.ContactsEntity;
import com.talker.api.exceptions.MessageFailure;
import com.talker.api.dao.MessagesDao;
import com.talker.api.dao.UserDao;
import com.talker.api.dto.requests.MessageRequest;
import com.talker.api.dto.MessageResponse;
import com.talker.api.entity.MessagesEntity;
import com.talker.api.entity.UserEntity;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import java.sql.Timestamp;
import java.util.ArrayList;
import java.util.List;

@Service
@Transactional
public class MessagesService {


    @Autowired
    public MessagesDao messagesDao;

    @Autowired
    public UserDao userDao;

    @Autowired
    private ContactsDao contactsDao;

    public List<MessagesEntity> listMessagesBetweenUsers(Integer senderId, Integer receiverId){
        return messagesDao.findTop30BySenderIdAndReceiverIdOrSenderIdAndReceiverIdOrderByIdDesc(
                senderId,
                receiverId,
                receiverId,
                senderId);
    }

    public List<MessagesEntity> listMessagesBetweenUsersLessThanId(Integer senderId, Integer receiverId, Integer idLessThan){
        return messagesDao.findTop30ByIdLessThanAndSenderIdAndReceiverIdOrIdLessThanAndSenderIdAndReceiverIdOrderByIdDesc(
                idLessThan,senderId, receiverId,
                idLessThan,receiverId, senderId);
    }

    public Integer checkIfUserHaveUnreadMessagesWithContact(Integer userId, Integer receiverId){
        Integer userHasUnreadMessagesWithOther = 0;

        List<MessagesEntity> allUnreadMessages = messagesDao.findAllBySenderIdAndReceiverIdAndSeenOrderByIdDesc(receiverId, userId, 0);

        for(MessagesEntity m : allUnreadMessages){
            userHasUnreadMessagesWithOther++;
        }

        return userHasUnreadMessagesWithOther;
    }



    public MessageResponse sendMessageToUser(UserEntity user, MessageRequest messageRequest){

        if (messageRequest.getReceiverId() == null || messageRequest.getMessage() == null) {
            throw new MessageFailure("No required data.");
        }
        try {
            userDao.findById(messageRequest.getReceiverId()).get();
        }catch (Exception e){
            throw new MessageFailure("User not found.");
        }

        if (messageRequest.getReceiverId() == user.getId()) {
            throw new MessageFailure("You can't send messages to yourself.");
        }

        ContactsEntity ce = contactsDao.findByUserIdAndContactId(messageRequest.getReceiverId() , user.getId());

        if(ce == null){
            //throw new MessageFailure("User doesn't have you in the contacts list");

            ContactRequest cr = new ContactRequest();
            cr.setUserId(messageRequest.getReceiverId());
            cr.setContactId(user.getId());
            cr.setAddedByUser(0);

            contactsDao.save(cr.toEntity());
        }

        messageRequest.setSenderId(user.getId());
        messageRequest.setSeen(0);
        MessagesEntity messageEntity = messagesDao.saveAndFlush(messageRequest.toEntity());


        MessageResponse mr = new MessageResponse();

        mr.setSenderId(user.getId());
        mr.setMessageId(messageEntity.getId());
        mr.setReceiverId(messageRequest.getReceiverId());
        mr.setMessage(messageRequest.getMessage());
        mr.setAddedTime(messageEntity.getAddedTime());
        mr.setSeen(messageRequest.getSeen());
        mr.setSeenTime(messageEntity.getSeenTime());


        return mr;
    }

    public List<MessageResponse> listMessages(UserEntity user, MessageRequest messageRequest) {

        if(messageRequest.getReceiverId() == null){
            throw new MessageFailure("No required data.");
        }

        List<MessagesEntity> messagesEntities;
        List<MessageResponse> messageResponses = new ArrayList<>();

        if(messageRequest.getLastMessageId() == null || messageRequest.getLastMessageId() == 0){
            messagesEntities = listMessagesBetweenUsers(user.getId(), messageRequest.getReceiverId());
        }else{
            messagesEntities = listMessagesBetweenUsersLessThanId(user.getId(), messageRequest.getReceiverId(), messageRequest.getLastMessageId());
        }

        for(MessagesEntity me: messagesEntities){
            MessageResponse mr = new MessageResponse();

            mr.setSenderId(me.getSenderId());
            mr.setMessageId(me.getId());
            mr.setReceiverId(me.getReceiverId());
            mr.setMessage(me.getMessage());
            mr.setAddedTime(me.getAddedTime());
            mr.setSeen(me.getSeen());
            mr.setSeenTime(me.getSeenTime());

            messageResponses.add(mr);

            if(me.getSenderId().equals(messageRequest.getReceiverId())) {
                me.setSeen(1);
                me.setSeenTime(new Timestamp(System.currentTimeMillis()));
            }
        }

        return messageResponses;
    }
}
