package com.talker.api.service;

import com.talker.api.entity.ContactsEntity;
import com.talker.api.exceptions.UserFailure;
import com.talker.api.dto.requests.DescriptionRequest;
import com.talker.api.dto.requests.StatusRequest;
import com.talker.api.dto.requests.UserRequest;
import com.talker.api.dto.UserResponse;
import com.talker.api.entity.UserEntity;
import com.talker.api.utils.PasswordHasher;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;


@Service
@Transactional
public class MainUserService {

    @Autowired
    public StatusesService statusService;

    @Autowired
    public DescriptionsService descriptionsService;

    @Autowired
    private ContactsService contactsService;

    @Autowired
    private  MessagesService messagesService;


    public UserResponse getUserData(UserEntity userEntity) {
        UserResponse u = new UserResponse();

        u.setId(userEntity.getId());
        u.setUsername(userEntity.getUsername());
        u.setFirstName(userEntity.getFirstName());
        u.setLastName(userEntity.getLastName());
        u.setEmail(userEntity.getEmail());
        u.setImage(userEntity.getImage());

        if(statusService.statusesDao.findTop1ByUserIdOrderByIdDesc(userEntity.getId()) != null)
            u.setStatus(statusService.statusesDao.findTop1ByUserIdOrderByIdDesc(userEntity.getId()).getStatus());
        if(descriptionsService.descriptionsDao.findTop1ByUserIdOrderByIdDesc(userEntity.getId()) != null)
            u.setDescription(descriptionsService.descriptionsDao.findTop1ByUserIdOrderByIdDesc(userEntity.getId()).getDescription());

        return u;
    }

    public UserResponse getContactDataAndCheckIfHaveUnreadMessagesWithMainUser(UserEntity contactUser, UserEntity mainUser) {
        UserResponse u = getUserData(contactUser);
        u.setUnreadMessages(messagesService.checkIfUserHaveUnreadMessagesWithContact(mainUser.getId(), contactUser.getId()));
        return u;
    }

    public UserResponse getUserDataWithContacts(UserEntity userEntity) {
        UserResponse ur = getUserData(userEntity);
        ur.setContacts(contactsService.getUserAllContacts(userEntity));
        return ur;
    }


    public void update(UserRequest userRequest, UserEntity user) {
        if (userRequest.getEmail() != null) user.setEmail(userRequest.getEmail());
        if (userRequest.getImage() != null) user.setImage(userRequest.getImage());
        if (userRequest.getFirstName() != null) user.setFirstName(userRequest.getFirstName());
        if (userRequest.getLastName() != null) user.setLastName(userRequest.getLastName());
        if (userRequest.getPassword() != null){
            user.setPassword(PasswordHasher.hash(userRequest.getPassword()));
            user.setToken(PasswordHasher.hash(PasswordHasher.hash(Math.random()+userRequest.getPassword())));
        }

        if (userRequest.getStatus() != null) {
            Integer s = userRequest.getStatus();
            if(s == null || s < 0 || s > 4){
                throw new UserFailure("Invalid status.");
            }
            StatusRequest sr = new StatusRequest();
            sr.setUserId(user.getId());
            sr.setStatus(userRequest.getStatus());
            statusService.statusesDao.save(sr.toEntity());
        }

        if(userRequest.getDescription() != null){
            DescriptionRequest dr = new DescriptionRequest();
            dr.setUserId(user.getId());
            dr.setDescription(userRequest.getDescription());
            descriptionsService.descriptionsDao.save(dr.toEntity());
        }
    }

}
