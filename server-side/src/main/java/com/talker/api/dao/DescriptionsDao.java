package com.talker.api.dao;

import com.talker.api.entity.DescriptionsEntity;
import com.talker.api.entity.StatusesEntity;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Component;


@Component
public interface DescriptionsDao extends JpaRepository<DescriptionsEntity, Integer> {

    public DescriptionsEntity findTop1ByUserIdOrderByIdDesc(Integer userId);
}
