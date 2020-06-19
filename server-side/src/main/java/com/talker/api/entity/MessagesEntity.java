package com.talker.api.entity;

import javax.persistence.*;
import java.sql.Timestamp;
import java.util.Objects;

@Entity
@Table(name = "messages", schema = "uwu", catalog = "")
public class MessagesEntity {
    private int id;
    private Integer senderId;
    private Integer receiverId;
    private String message;
    private Integer seen;
    private Timestamp seenTime;
    private Timestamp addedTime;

    @Id
    @Column(name = "id", nullable = false)
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    public int getId() {
        return id;
    }

    public void setId(int id) {
        this.id = id;
    }

    @Basic
    @Column(name = "sender_id", nullable = true)
    public Integer getSenderId() {
        return senderId;
    }

    public void setSenderId(Integer senderId) {
        this.senderId = senderId;
    }

    @Basic
    @Column(name = "receiver_id", nullable = true)
    public Integer getReceiverId() {
        return receiverId;
    }

    public void setReceiverId(Integer receiverId) {
        this.receiverId = receiverId;
    }

    @Basic
    @Column(name = "message", nullable = true, length = 2048)
    public String getMessage() {
        return message;
    }

    public void setMessage(String message) {
        this.message = message;
    }

    @Basic
    @Column(name = "seen", nullable = true)
    public Integer getSeen() {
        return seen;
    }

    public void setSeen(Integer seen) {
        this.seen = seen;
    }

    @Basic
    @Column(name = "seen_time", nullable = true)
    public Timestamp getSeenTime() {
        return seenTime;
    }

    public void setSeenTime(Timestamp seenTime) {
        this.seenTime = seenTime;
    }

    @Basic
    @Column(name = "added_time", nullable = true)
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
        MessagesEntity that = (MessagesEntity) o;
        return id == that.id &&
                Objects.equals(senderId, that.senderId) &&
                Objects.equals(receiverId, that.receiverId) &&
                Objects.equals(message, that.message) &&
                Objects.equals(seen, that.seen) &&
                Objects.equals(seenTime, that.seenTime) &&
                Objects.equals(addedTime, that.addedTime);
    }

    @Override
    public int hashCode() {
        return Objects.hash(id, senderId, receiverId, message, seen, seenTime, addedTime);
    }
}
