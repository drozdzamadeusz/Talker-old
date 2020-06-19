package com.talker.api.exceptions;

import org.springframework.http.HttpStatus;
import org.springframework.web.bind.annotation.ResponseStatus;

@ResponseStatus(HttpStatus.FORBIDDEN)
public class RegisterFailure extends RuntimeException {
    public RegisterFailure(String message) {
        super(message);
    }

    public RegisterFailure(String message, Throwable cause) {
        super(message, cause);
    }
}
