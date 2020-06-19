package com.talker.api.ws;

import com.talker.api.dto.StatusReponse;
import com.talker.api.dto.TokenResponse;
import com.talker.api.exceptions.AuthenticationFailure;
import com.talker.api.exceptions.RegisterFailure;
import com.talker.api.config.JwtTokenUtil;
import com.talker.api.dto.requests.UserRequest;
import com.talker.api.entity.UserEntity;
import com.talker.api.service.ContactsService;
import com.talker.api.service.UserService;
import com.talker.api.utils.PasswordHasher;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.web.bind.annotation.*;

import javax.servlet.http.HttpServletRequest;
import javax.validation.Valid;
import java.util.Objects;
import java.util.Random;

@RestController
@RequestMapping("/auth/")
@CrossOrigin(origins = "http://localhost", maxAge = 3600)
public class AuthController {

    @Autowired
    private UserService userService;

    @Autowired
    private ContactsService contactsService;

    @Autowired
    private JwtTokenUtil jwtTokenUtil;


    @PostMapping("login")
    public TokenResponse login(@RequestBody UserRequest login, HttpServletRequest request) throws AuthenticationFailure {
        String jwtToken = "";

        if (login.getUsername() == null || login.getPassword() == null) {
            throw new AuthenticationFailure("Please fill in username and password.");
        }

        String username = login.getUsername();
        String password = login.getPassword();

        UserEntity user = userService.userDao.findTop1ByUsername(username);

        if (user == null) {
            //throw new AuthenticationFailure("User not found.");
            throw new AuthenticationFailure("Please verify your login and password.");
        }

        String pwd = user.getPassword();

        if (!Objects.equals(PasswordHasher.hash(password), pwd)) {
            throw new AuthenticationFailure("Please verify your login and password.");
        }

        TokenResponse token = new TokenResponse();
        token.setToken(jwtTokenUtil.generateToken(user, request));

        return token;
    }


    private static String generateColor(Random r) {
        final char [] hex = { '0', '1', '2', '3', '4', '5', '6', '7',
                '8', '9', 'a', 'b', 'c', 'd', 'e', 'f' };
        char [] s = new char[6];
        int     n = r.nextInt(0x1000000);

        for (int i=0;i<6;i++) {
            s[i] = hex[n & 0xf];
            n >>= 4;
        }
        return new String(s);
    }


    @PostMapping("register")
    public StatusReponse register(@Valid @RequestBody UserRequest userRequest) throws AuthenticationFailure {

        if (    userRequest.getUsername() == null ||
                userRequest.getPassword() == null ||
                userRequest.getEmail() == null ||
                userRequest.getFirstName() == null ||
                userRequest.getLastName() == null ) {
            throw new RegisterFailure("Please complete all required data.");
        }

        UserEntity userByUsername = userService.userDao.findTop1ByUsername(userRequest.getUsername());

        if (userByUsername != null) {
            throw new RegisterFailure("Username already taken.");
        }

        UserEntity userByEmail = userService.userDao.findTop1ByEmail(userRequest.getEmail());

        if (userByEmail != null) {
            throw new RegisterFailure("Email already registered.");
        }

        if(userRequest.getPassword().length() < 4){
            throw new RegisterFailure("Password is too short.");
        }

        if(userRequest.getImage() == null || userRequest.getImage().equals("")){
            userRequest.setImage("https://eu.ui-avatars.com/api/?format=svg&bold=false&size=200&color="+generateColor(new Random())+"&background="+generateColor(new Random())+"&name="+userRequest.getFirstName().charAt(0)+userRequest.getLastName().charAt(0)+"&font-size=0.45");
        }

        userService.userDao.save(userRequest.toEntity());

        /*ContactRequest cr = new ContactRequest();
        cr.setUserId(ue.getId());
        cr.setContactId(1);
        cr.setAddedByUser(0);

        contactsService.contactsDao.save(cr.toEntity());*/

        /*UserRequest ur = new UserRequest();
        ur.setEmail("amadro@gg.pl");

        contactsService.addContact(ue, ur);*/

        StatusReponse sr  = new StatusReponse();
        sr.setError("Successful Registration");
        sr.setMessage("The user has been added successfully. <br />Please go to the login window to continue.");

        return sr;
    }

}
