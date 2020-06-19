package com.talker.api.dao;

import com.talker.api.entity.ContactsEntity;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Component;

import java.util.List;

@Component
public interface ContactsDao extends JpaRepository<ContactsEntity, Integer> {

    List<ContactsEntity> findAllByUserId(int id);

    void deleteByUserIdAndContactId(int userId, int contactId);

    ContactsEntity findByUserIdAndContactId(int id, int contactId);
}
