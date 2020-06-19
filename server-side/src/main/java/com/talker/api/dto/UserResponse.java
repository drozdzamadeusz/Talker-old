package com.talker.api.dto;


import java.util.List;

public class UserResponse {
    private Integer id;
    private String username;

    private String email;
    private String image;
    private String firstName;
    private String lastName;

    private Integer status;
    private String description;
    private Integer unreadMessages;

    private Integer addedByUser;

    public List<UserResponse> contacts;

    public int getId() { return id; }

    public void setId(int id) { this.id = id; }

    public String getUsername() {
        return username;
    }

    public void setUsername(String username) {
        this.username = username;
    }

    public String getImage() {
        return image;
    }

    public void setImage(String image) {
        this.image = image;
    }

    public String getEmail() { return email;}

    public void setEmail(String email) { this.email = email;}

    public String getFirstName() { return firstName; }

    public void setFirstName(String firstName) {this.firstName = firstName; }

    public String getLastName() { return lastName;}

    public void setLastName(String lastName) {this.lastName = lastName; }

    public Integer getStatus() {
        return status;
    }

    public void setStatus(Integer status) {
        this.status = status;
    }

    public String getDescription() { return description;  }

    public void setDescription(String description) {  this.description = description; }

    public Integer getUnreadMessages() {
        return unreadMessages;
    }

    public void setUnreadMessages(Integer unreadMessages) {
        this.unreadMessages = unreadMessages;
    }

    public List<UserResponse> getContacts() {
        return contacts;
    }

    public void setContacts(List<UserResponse> contacts) {
        this.contacts = contacts;
    }

    public Integer getAddedByUser() {
        return addedByUser;
    }

    public void setAddedByUser(Integer addedByUser) {
        this.addedByUser = addedByUser;
    }
}
