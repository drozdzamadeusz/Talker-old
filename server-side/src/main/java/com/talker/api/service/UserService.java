package com.talker.api.service;


import com.talker.api.dao.UserDao;
import com.talker.api.dto.UserResponse;
import com.talker.api.entity.UserEntity;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

@Service
@Transactional
public class UserService {

    @Autowired
    public UserDao userDao;

    /* ZASLEPKA: TU NIE MOZE BYC ŻADNYCH METOD BO SIE SCRASCHUJE - metody z userService przeniesione do mainUserService */
}
