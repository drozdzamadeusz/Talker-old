package com.talker.api.entity;

import javax.persistence.*;
import java.sql.Timestamp;
import java.util.Objects;

@Entity
@Table(name = "contacts", schema = "uwu", catalog = "")
public class ContactsEntity {
    private int id;
    private int userId;
    private int contactId;
    private Integer addedByUser;
    private Timestamp addedTime;

    @Id
    @Column(name = "id", nullable = false)
    public int getId() {
        return id;
    }

    public void setId(int id) {
        this.id = id;
    }

    @Basic
    @Column(name = "user_id", nullable = false)
    public int getUserId() {
        return userId;
    }

    public void setUserId(int userId) {
        this.userId = userId;
    }

    @Basic
    @Column(name = "contact_id", nullable = false)
    public int getContactId() {
        return contactId;
    }

    public void setContactId(int contactId) {
        this.contactId = contactId;
    }

    @Basic
    @Column(name = "added_by_user", nullable = true)
    public Integer getAddedByUser() {
        return addedByUser;
    }

    public void setAddedByUser(Integer addedByUser) {
        this.addedByUser = addedByUser;
    }

    @Basic
    @Column(name = "added_time", nullable = false)
    public Timestamp getAddedTime() {
        return addedTime;
    }

    public void setAddedTime(Timestamp addedTime) {
        this.addedTime = addedTime;
    }

    @Override
    public boolean equals(Object o) {
        if (this == o) return true;
        if (o == null || getClass() != o.getClass()) return false;
        ContactsEntity that = (ContactsEntity) o;
        return id == that.id &&
                userId == that.userId &&
                contactId == that.contactId &&
                Objects.equals(addedByUser, that.addedByUser) &&
                Objects.equals(addedTime, that.addedTime);
    }

    @Override
    public int hashCode() {
        return Objects.hash(id, userId, contactId, addedByUser, addedTime);
    }
}
