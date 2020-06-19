package com.talker.api.exceptions;

import org.springframework.http.HttpStatus;
import org.springframework.web.bind.annotation.ResponseStatus;

@ResponseStatus(HttpStatus.FORBIDDEN)
public class ContactsFaliure extends RuntimeException {
    public ContactsFaliure(String message) {
        super(message);
    }

    public ContactsFaliure(String message, Throwable cause) {
        super(message, cause);
    }
}
