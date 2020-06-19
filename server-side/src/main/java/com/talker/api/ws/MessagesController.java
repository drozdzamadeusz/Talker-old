package com.talker.api.ws;

import com.talker.api.dto.StatusReponse;
import com.talker.api.exceptions.AuthenticationFailure;
import com.talker.api.config.CurrentUser;
import com.talker.api.dto.requests.MessageRequest;
import com.talker.api.dto.MessageResponse;
import com.talker.api.entity.UserEntity;
import com.talker.api.service.MessagesService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;

import javax.servlet.http.HttpServletRequest;
import java.util.List;

@RestController
@CurrentUser
@RequestMapping("/api/messages/")
public class MessagesController extends UserAuthorized{

    @Autowired
    private MessagesService messagesService;

    @PostMapping("send")
    public MessageResponse sendMessage(@RequestBody MessageRequest messageRequest, HttpServletRequest request) throws AuthenticationFailure {
        UserEntity user = getUserFromRequest(request);

        return messagesService.sendMessageToUser(user, messageRequest);
    }

    @PostMapping("list")
    public List<MessageResponse> messagesList(@RequestBody MessageRequest messageRequest, HttpServletRequest request) throws AuthenticationFailure {
       UserEntity user = getUserFromRequest(request);
       return messagesService.listMessages(user, messageRequest);
    }
}
