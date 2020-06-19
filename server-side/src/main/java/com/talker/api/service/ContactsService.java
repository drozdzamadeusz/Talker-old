package com.talker.api.service;

import com.talker.api.exceptions.ContactsFaliure;
import com.talker.api.dao.ContactsDao;
import com.talker.api.dao.UserDao;
import com.talker.api.dto.requests.ContactRequest;
import com.talker.api.dto.requests.UserRequest;
import com.talker.api.dto.UserResponse;
import com.talker.api.entity.ContactsEntity;
import com.talker.api.entity.UserEntity;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import java.util.ArrayList;
import java.util.List;


@Service
@Transactional
public class ContactsService {

    @Autowired
    public ContactsDao contactsDao;

    @Autowired
    public UserDao userDao;


    @Autowired
    public MainUserService mainUserService;


    public List<UserResponse> getUserAllContacts(UserEntity userEntity){

        List<ContactsEntity> allContacts = contactsDao.findAllByUserId(userEntity.getId());
        List<UserResponse> users = new ArrayList<>();

        for(ContactsEntity contact: allContacts){
            UserEntity ue = userDao.findById(contact.getContactId()).get();

            UserResponse ur = mainUserService.getContactDataAndCheckIfHaveUnreadMessagesWithMainUser(ue, userEntity);

            ur.setAddedByUser(contact.getAddedByUser());

            users.add(ur);
        }

        return users;
    }

    public void addContact(UserEntity userEntity, UserRequest userRequest){
        UserEntity ue  = userDao.findTop1ByUsername(userRequest.getUsername());

        if(ue == null)
            ue  = userDao.findTop1ByEmail(userRequest.getEmail());
        if(ue == null){
            throw new ContactsFaliure("User not found.");
        }

        if(userEntity.getId() == ue.getId()){
            throw new ContactsFaliure("You can't add yourself to the contacts list.");
        }

        List<ContactsEntity> allContacts = contactsDao.findAllByUserId(userEntity.getId());

        for(ContactsEntity contact: allContacts){
            if(ue.getId() == contact.getContactId() && contact.getAddedByUser().equals(0)){
                contact.setAddedByUser(1);
                return;
            }
            if(ue.getId() == contact.getContactId() && contact.getAddedByUser().equals(1)){
                throw new ContactsFaliure("Already on the contacts list.");
            }
        }

        ContactRequest cr = new ContactRequest();
        cr.setUserId(userEntity.getId());
        cr.setContactId(ue.getId());
        cr.setAddedByUser(1);
        contactsDao.save(cr.toEntity());
    }

    public void removeContact(UserEntity userEntity, UserRequest userRequest) {
        UserEntity ue  = userDao.findById(userRequest.getId()).get();
        if(ue == null){
            throw new ContactsFaliure("Contact not found.");
        }

        contactsDao.deleteByUserIdAndContactId(userEntity.getId(), ue.getId());
    }
}
