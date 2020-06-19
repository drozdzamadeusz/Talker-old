package com.talker.api.exceptions;

import org.springframework.http.HttpStatus;
import org.springframework.web.bind.annotation.ResponseStatus;

@ResponseStatus(HttpStatus.UNAUTHORIZED)
public class AuthenticationFailure extends RuntimeException {
    public AuthenticationFailure(String message) {
        super(message);
    }

    public AuthenticationFailure(String message, Throwable cause) {
        super(message, cause);
    }
}
