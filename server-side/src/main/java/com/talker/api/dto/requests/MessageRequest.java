package com.talker.api.dto.requests;

import com.talker.api.entity.MessagesEntity;

import java.sql.Timestamp;

public class MessageRequest {
    private Integer senderId;
    private Integer receiverId;
    private String message;
    private Timestamp addedTime;
    private Integer lastMessageId;
    private Integer seen;


    public Integer getSenderId() {
        return senderId;
    }

    public void setSenderId(Integer senderId) {
        this.senderId = senderId;
    }

    public Integer getReceiverId() {
        return receiverId;
    }

    public void setReceiverId(Integer receiverId) {
        this.receiverId = receiverId;
    }

    public String getMessage() {
        return message;
    }

    public void setMessage(String message) {
        this.message = message;
    }

    public Timestamp getAddedTime() {
        return addedTime;
    }

    public void setAddedTime(Timestamp addedTime) {
        this.addedTime = addedTime;
    }

    public Integer getLastMessageId() {
        return lastMessageId;
    }

    public void setLastMessageId(Integer lastMessageId) {
        this.lastMessageId = lastMessageId;
    }

    public Integer getSeen() {
        return seen;
    }

    public void setSeen(Integer seen) {
        this.seen = seen;
    }

    public MessagesEntity toEntity(){
        MessagesEntity messagesEntity = new MessagesEntity();
        messagesEntity.setSenderId(senderId);
        messagesEntity.setReceiverId(receiverId);
        messagesEntity.setMessage(message);
        messagesEntity.setAddedTime(new Timestamp(System.currentTimeMillis()));
        messagesEntity.setSeen(seen);
        return messagesEntity;
    }
}
