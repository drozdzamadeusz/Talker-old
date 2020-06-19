package com.talker.api.dto.requests;

import com.talker.api.entity.UserEntity;
import com.talker.api.utils.PasswordHasher;

import java.sql.Timestamp;


public class UserRequest {
    private int id;
    private String username;
    private String password;

    private String email;
    private String image;
    private String firstName;
    private String lastName;

    private Integer status;
    private String description;

    private Integer userType = 1;

    public int getId() { return id; }

    public void setId(int id) { this.id = id;}

    public String getUsername() {
        return username;
    }

    public void setUsername(String username) {
        this.username = username;
    }

    public String getEmail() { return email;}

    public void setEmail(String email) { this.email = email;}

    public String getImage() { return image; }

    public void setImage(String image) { this.image = image;}

    public String getFirstName() { return firstName; }

    public void setFirstName(String firstName) {this.firstName = firstName; }

    public String getLastName() { return lastName;}

    public void setLastName(String lastName) {this.lastName = lastName; }

    public String getPassword() {
        return password;
    }

    public void setPassword(String password) {
        this.password = password;
    }

    public Integer getStatus() {return status; }

    public void setStatus(Integer status) { this.status = status; }

    public String getDescription() { return description;  }

    public void setDescription(String description) {  this.description = description; }

    public UserEntity toEntity(){
        UserEntity user = new UserEntity();
        user.setPassword(PasswordHasher.hash(password));
        user.setUsername(username);
        user.setEmail(email);
        user.setFirstName(firstName);
        user.setLastName(lastName);
        user.setRegisteredDate(new Timestamp(System.currentTimeMillis()));
        user.setToken(PasswordHasher.hash(Math.random()+password));
        user.setImage(image);
        return user;
    }


}
