package com.talker.api.dto;

import java.sql.Timestamp;

public class StatusReponse {
    public Timestamp timestamp;

    public Integer status;

    public String error;

    public String message;

    public String path;

    public Timestamp getTimestamp() {
        return timestamp;
    }

    public void setTimestamp(Timestamp timestamp) {
        this.timestamp = timestamp;
    }

    public Integer getStatus() {
        return status;
    }

    public void setStatus(Integer status) {
        this.status = status;
    }

    public String getError() {
        return error;
    }

    public void setError(String error) {
        this.error = error;
    }

    public String getMessage() {
        return message;
    }

    public void setMessage(String message) {
        this.message = message;
    }

    public String getPath() {
        return path;
    }

    public void setPath(String path) {
        this.path = path;
    }

    public static StatusReponse statusOk(){
        StatusReponse sr = new StatusReponse();

        sr.setMessage("ok");
        sr.setTimestamp(new Timestamp(System.currentTimeMillis()));
        sr.setStatus(200);


        return sr;
    }


}
