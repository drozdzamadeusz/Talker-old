package com.talker.api.dto.requests;

import com.talker.api.entity.ContactsEntity;

import java.sql.Timestamp;

public class ContactRequest {
    private int userId;
    private int contactId;
    private Integer addedByUser;
    private Timestamp addedTime;

    public void setUserId(int userId) {
        this.userId = userId;
    }

    public int getContactId() {
        return contactId;
    }

    public void setContactId(int contactId) {
        this.contactId = contactId;
    }

    public Integer getAddedByUser() {
        return addedByUser;
    }

    public void setAddedByUser(Integer addedByUser) {
        this.addedByUser = addedByUser;
    }

    public Timestamp getAddedTime() {
        return addedTime;
    }

    public void setAddedTime(Timestamp addedTime) {
        this.addedTime = addedTime;
    }

    public ContactsEntity toEntity(){
        ContactsEntity contact = new ContactsEntity();
        contact.setUserId(userId);
        contact.setContactId(contactId);
        contact.setAddedByUser(addedByUser);
        contact.setAddedTime(new Timestamp(System.currentTimeMillis()));
        return contact;
    }
}
