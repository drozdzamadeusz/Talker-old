package com.talker.api.dao;

import com.talker.api.entity.MessagesEntity;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Component;

import java.util.List;

@Component
public interface MessagesDao extends JpaRepository<MessagesEntity, Integer> {

    List<MessagesEntity> findTop30BySenderIdAndReceiverIdOrSenderIdAndReceiverIdOrderByIdDesc(Integer senderId, Integer receiverId,
                                                                                              Integer senderId1, Integer receiverId1);


    List<MessagesEntity> findTop30ByIdLessThanAndSenderIdAndReceiverIdOrIdLessThanAndSenderIdAndReceiverIdOrderByIdDesc(Integer idLessThan,Integer senderId, Integer receiverId,
                                                                                                                          Integer idLessThan1,Integer senderId1, Integer receiverId1);


    List<MessagesEntity> findAllBySenderIdAndReceiverIdAndSeenOrderByIdDesc(Integer senderId, Integer receiverId, Integer seen);
}


