package com.talker.api.enums;

public enum STATUS_UPDATE_RESPONSE{
    UPDATE_TOO_SOON(-1),
    NOTHING_NEW(0),
    NEW_MESSAGE(1),
    FRIENDS_UPDATE(2),
    NEW_MESSAGE_AND_FRIENDS_UPDATE(3),
    NEW_FRIEND(4),
    FRIEND_REMOVED(5),
    FIRST_UPDATE(6);

    private int value;

    private STATUS_UPDATE_RESPONSE(int value) {
        this.value = value;
    }

    public int getValue() {
        return value;
    }


    private String messagesFromUsers = "-";

    public String getMessagesFromUsers() {
        return messagesFromUsers;
    }

    public void setMessagesFromUsers(String messagesFromUsers) {
        this.messagesFromUsers = messagesFromUsers;
    }

    public void appendUserToMessagesFormUsers(int user) {
        this.messagesFromUsers += (String.valueOf(user)+"-");
    }


    public String getResponseValue(){
        return ";"+getValue()+";"+getMessagesFromUsers();
    }

}
