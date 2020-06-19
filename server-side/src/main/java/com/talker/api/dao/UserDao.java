package com.talker.api.dao;

import com.talker.api.entity.UserEntity;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Component;

@Component
public interface UserDao extends JpaRepository<UserEntity, Integer> {

    UserEntity findTop1ByUsername(String username);

    UserEntity findTop1ByEmail(String email);

}
