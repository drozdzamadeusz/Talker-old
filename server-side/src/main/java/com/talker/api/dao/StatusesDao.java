package com.talker.api.dao;

import com.talker.api.entity.StatusesEntity;
import com.talker.api.entity.UserEntity;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Component;


@Component
public interface StatusesDao extends JpaRepository<StatusesEntity, Integer> {

    public StatusesEntity findTop1ByUserIdOrderByIdDesc(Integer userId);
}
