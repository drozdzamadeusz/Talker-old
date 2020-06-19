package com.talker.api.ws;

import com.talker.api.dto.StatusReponse;
import com.talker.api.exceptions.AuthenticationFailure;
import com.talker.api.config.CurrentUser;
import com.talker.api.dto.requests.UserRequest;
import com.talker.api.dto.UserResponse;
import com.talker.api.entity.UserEntity;
import com.talker.api.service.MainUserService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;

import javax.servlet.http.HttpServletRequest;
import javax.validation.Valid;

@RestController
@CurrentUser
@RequestMapping("/api/user/")
public class UserController extends UserAuthorized{

    @Autowired
    private MainUserService mainUserService;

    @PostMapping("me")
    public UserResponse me(HttpServletRequest request) throws AuthenticationFailure {
        UserEntity user = getUserFromRequest(request);
        return mainUserService.getUserDataWithContacts(user);
    }

    @PostMapping("update")
    public StatusReponse update(@Valid @RequestBody UserRequest userRequest, HttpServletRequest request) throws AuthenticationFailure {
        UserEntity user = getUserFromRequest(request);

        mainUserService.update(userRequest, user);

        return StatusReponse.statusOk();

    }
}
