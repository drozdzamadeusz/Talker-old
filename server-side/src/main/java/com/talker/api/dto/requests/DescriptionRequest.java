package com.talker.api.dto.requests;

import com.talker.api.entity.DescriptionsEntity;

import java.sql.Timestamp;

public class DescriptionRequest {

    private Integer userId;
    private String description;
    private Timestamp addedTime;

    public Integer getUserId() {
        return userId;
    }

    public void setUserId(Integer userId) {
        this.userId = userId;
    }

    public String getDescription() {
        return description;
    }

    public void setDescription(String description) {
        this.description = description;
    }

    public Timestamp getAddedTime() {
        return addedTime;
    }

    public void setAddedTime(Timestamp addedTime) {
        this.addedTime = addedTime;
    }

    public DescriptionsEntity toEntity(){
        DescriptionsEntity entity = new DescriptionsEntity();
        entity.setUserId(userId);
        entity.setDescription(description);
        entity.setAddedTime(new Timestamp(System.currentTimeMillis()));
        return entity;
    }
}
