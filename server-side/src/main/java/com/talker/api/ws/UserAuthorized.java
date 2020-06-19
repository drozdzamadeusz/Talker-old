package com.talker.api.ws;

import com.talker.api.config.CurrentUser;
import com.talker.api.config.JwtFilter;
import com.talker.api.config.JwtTokenUtil;
import com.talker.api.entity.UserEntity;
import com.talker.api.exceptions.AuthenticationFailure;
import com.talker.api.service.UserService;
import org.springframework.beans.factory.annotation.Autowired;

import javax.servlet.http.HttpServletRequest;

@CurrentUser
public class UserAuthorized {

    @Autowired
    private JwtTokenUtil jwtTokenUtil;

    @Autowired
    private UserService userService;


    public UserEntity getUserFromRequest(HttpServletRequest request) {


        if(!jwtTokenUtil.validateRequest(request)){
            throw new AuthenticationFailure("Client IP changed or token expired");
        }
        UserEntity ue = userService.userDao.findById(jwtTokenUtil.getUserIdFromRequest(request)).get();
        //System.out.println(jwtTokenUtil.getUserStaticTokenFromRequest(request) + "<-");

        String token  = JwtFilter.getJwtFromRequest(request);
        //System.out.println(jwtTokenUtil.getExpirationDateFromToken(token));

        if(!jwtTokenUtil.getUserStaticTokenFromRequest(request).equals(ue.getToken())){
            throw new AuthenticationFailure("Session expired.");
        }
        return ue;
    }

}
