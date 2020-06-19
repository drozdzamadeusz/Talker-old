package com.talker.api.exceptions;

import org.springframework.http.HttpStatus;
import org.springframework.web.bind.annotation.ResponseStatus;

@ResponseStatus(HttpStatus.FORBIDDEN)
public class UserFailure extends RuntimeException {
    public UserFailure(String message) {
        super(message);
    }

    public UserFailure(String message, Throwable cause) {
        super(message, cause);
    }
}
