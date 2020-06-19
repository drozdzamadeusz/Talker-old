package com.talker.api.dto.requests;

import com.talker.api.entity.StatusesEntity;

import java.sql.Timestamp;

public class StatusRequest {

    private Integer userId;
    private Integer status;
    private Timestamp addedTime;

    public Integer getUserId() {
        return userId;
    }

    public void setUserId(Integer userId) {
        this.userId = userId;
    }

    public Integer getStatus() {
        return status;
    }

    public void setStatus(Integer status) {
        this.status = status;
    }

    public Timestamp getAddedTime() {
        return addedTime;
    }

    public void setAddedTime(Timestamp addedTime) {
        this.addedTime = addedTime;
    }

    public StatusesEntity toEntity(){
        StatusesEntity entity = new StatusesEntity();
        entity.setUserId(userId);
        entity.setStatus(status);
        entity.setAddedTime(new Timestamp(System.currentTimeMillis()));
        return entity;
    }
}
