package com.talker.api.exceptions;

import org.springframework.http.HttpStatus;
import org.springframework.web.bind.annotation.ResponseStatus;

@ResponseStatus(HttpStatus.FORBIDDEN)
public class MessageFailure extends RuntimeException {
    public MessageFailure(String message) {
        super(message);
    }

    public MessageFailure(String message, Throwable cause) {
        super(message, cause);
    }
}
