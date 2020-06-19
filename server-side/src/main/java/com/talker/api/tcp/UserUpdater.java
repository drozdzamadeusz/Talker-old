package com.talker.api.tcp;

import com.talker.api.dto.UserResponse;
import com.talker.api.entity.UserEntity;
import com.talker.api.enums.STATUS_UPDATE_RESPONSE;
import com.talker.api.service.ContactsService;

import java.sql.Timestamp;
import java.util.*;


public class UserUpdater{

    public static int minimumRefreshDelay = 2000;

    public Timestamp lastUpdate;
    public UserEntity user;

    public Timestamp lastUserListChecked;

    boolean firstUpdate;

    List<UserResponse> contacts;

    ContactsService contactsService;

    public UserUpdater(Timestamp lastUpdate, UserEntity user, ContactsService contactsService) {
        this.lastUpdate = lastUpdate;
        this.user = user;
        this.contactsService = contactsService;
        firstUpdate = true;
    }

    public STATUS_UPDATE_RESPONSE update(Timestamp currTime){


        STATUS_UPDATE_RESPONSE status_update_response = STATUS_UPDATE_RESPONSE.NOTHING_NEW;

        StringBuilder messagesFormUsers = new StringBuilder("-");

        //log.info(currTime.getTime() - lastUpdate.getTime());

        if(!firstUpdate && (currTime.getTime() - lastUpdate.getTime() <= minimumRefreshDelay)){
            status_update_response = STATUS_UPDATE_RESPONSE.UPDATE_TOO_SOON;
        }else if(firstUpdate || (currTime.getTime() - lastUpdate.getTime() > minimumRefreshDelay)){

            if(contacts == null || firstUpdate) {

                contacts = contactsService.getUserAllContacts(user);
                status_update_response  =  STATUS_UPDATE_RESPONSE.FIRST_UPDATE;

                firstUpdate = false;
            }else {
                List<UserResponse> newContactsList = contactsService.getUserAllContacts(user);

                boolean newFriend= false;

                if(newContactsList.size() != contacts.size()){

                    if(newContactsList.size() > contacts.size()){
                        status_update_response  =  STATUS_UPDATE_RESPONSE.NEW_FRIEND;

                        newContactsList.removeAll(contacts);

                        for (UserResponse ur : newContactsList) {
                            if (ur.getUnreadMessages() > 0) {
                                messagesFormUsers.append(ur.getId()).append("-");
                            }
                        }
                    }else {
                        status_update_response = STATUS_UPDATE_RESPONSE.FRIEND_REMOVED;

                        for (int i = 0; i < contacts.size(); i++)
                        {
                            int j;
                            for (j = 0; j < newContactsList.size(); j++)
                                if (contacts.get(i).getId() == newContactsList.get(j).getId() )
                                    break;

                            if (j == newContactsList.size())
                                messagesFormUsers.append(contacts.get(i).getId()).append("-");
                        }



                    }

                }else {

                    boolean newMessages = false;
                    boolean friendsUpdate = false;

                    for (int i = 0; i < contacts.size(); i++) {

                        UserResponse lastUpdateContact = contacts.get(i);
                        UserResponse newUpdateContact = newContactsList.get(i);

                        if (!lastUpdateContact.getUnreadMessages().equals(newUpdateContact.getUnreadMessages())){
                            newMessages = true;
                            messagesFormUsers.append(newUpdateContact.getId()).append("-");
                        }
                        if(		((lastUpdateContact.getUsername() != null && newUpdateContact.getUsername() == null) || (lastUpdateContact.getUsername() == null && newUpdateContact.getUsername() != null) ||
                                (lastUpdateContact.getUsername() != null && newUpdateContact.getUsername() != null)  &&   (!lastUpdateContact.getUsername().equals(newUpdateContact.getUsername()))) ||

                                ((lastUpdateContact.getEmail() != null && newUpdateContact.getEmail() == null) || (lastUpdateContact.getEmail() == null && newUpdateContact.getEmail() != null) ||
                                        (lastUpdateContact.getEmail() != null && newUpdateContact.getEmail() != null)  &&   (!lastUpdateContact.getEmail().equals(newUpdateContact.getEmail()))) ||

                                ((lastUpdateContact.getImage() != null && newUpdateContact.getImage() == null) || (lastUpdateContact.getImage() == null && newUpdateContact.getImage() != null) ||
                                        (lastUpdateContact.getImage() != null && newUpdateContact.getImage() != null)  &&   (!lastUpdateContact.getImage().equals(newUpdateContact.getImage()))) ||

                                ((lastUpdateContact.getFirstName() != null && newUpdateContact.getFirstName() == null) || (lastUpdateContact.getFirstName() == null && newUpdateContact.getFirstName() != null) ||
                                        (lastUpdateContact.getFirstName() != null && newUpdateContact.getFirstName() != null)  &&   (!lastUpdateContact.getFirstName().equals(newUpdateContact.getFirstName()))) ||

                                ((lastUpdateContact.getLastName() != null && newUpdateContact.getLastName() == null) || (lastUpdateContact.getLastName() == null && newUpdateContact.getLastName() != null) ||
                                        (lastUpdateContact.getLastName() != null && newUpdateContact.getLastName() != null)  &&   (!lastUpdateContact.getLastName().equals(newUpdateContact.getLastName()))) ||

                                ((lastUpdateContact.getStatus() != null && newUpdateContact.getStatus() == null) || (lastUpdateContact.getStatus() == null && newUpdateContact.getStatus() != null) ||
                                        (lastUpdateContact.getStatus() != null && newUpdateContact.getStatus() != null)  &&   (!lastUpdateContact.getStatus().equals(newUpdateContact.getStatus()))) ||

                                ((lastUpdateContact.getDescription() != null && newUpdateContact.getDescription() == null) || (lastUpdateContact.getDescription() == null && newUpdateContact.getDescription() != null) ||
                                        (lastUpdateContact.getDescription() != null && newUpdateContact.getDescription() != null)  &&   (!lastUpdateContact.getDescription().equals(newUpdateContact.getDescription()))) ||

                                ((lastUpdateContact.getAddedByUser() != null && newUpdateContact.getAddedByUser() == null) || (lastUpdateContact.getAddedByUser() == null && newUpdateContact.getAddedByUser() != null) ||
                                        (lastUpdateContact.getAddedByUser() != null && newUpdateContact.getAddedByUser() != null)  &&   (!lastUpdateContact.getAddedByUser().equals(newUpdateContact.getAddedByUser())))

                        ){
                            friendsUpdate = true;
                        }
                        //if(newMessages && friendsUpdate) break;
                    }
                    if(newMessages && friendsUpdate){
                        status_update_response = STATUS_UPDATE_RESPONSE.NEW_MESSAGE_AND_FRIENDS_UPDATE;
                    }else if(newMessages){
                        status_update_response = STATUS_UPDATE_RESPONSE.NEW_MESSAGE;
                    }else if(friendsUpdate){
                        status_update_response = STATUS_UPDATE_RESPONSE.FRIENDS_UPDATE;
                    }

                }
                contacts = newContactsList;
            }
            this.lastUpdate = currTime;
        }

        status_update_response.setMessagesFromUsers(messagesFormUsers.toString());
        return status_update_response;
    }
}

