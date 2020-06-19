package com.talker.api.dto;

import java.sql.Timestamp;

public class MessageResponse {
    private Integer messageId;
    private Integer senderId;
    private Integer receiverId;
    private String message;
    private Integer seen;
    private Timestamp seenTime;
    private Timestamp addedTime;

    public Integer getMessageId() {
        return messageId;
    }

    public void setMessageId(Integer messageId) {
        this.messageId = messageId;
    }

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

    public Integer getSeen() {
        return seen;
    }

    public void setSeen(Integer seen) {
        this.seen = seen;
    }

    public Timestamp getSeenTime() {
        return seenTime;
    }

    public void setSeenTime(Timestamp seenTime) {
        this.seenTime = seenTime;
    }
}

