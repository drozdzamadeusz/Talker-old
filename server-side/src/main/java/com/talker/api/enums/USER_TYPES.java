package com.talker.api.enums;

public enum USER_TYPES {
    UZYTKOWNIK(1, "Użytkownik"),
    ADMINISTRATOR(2, "Administrator");

    int id;
    String description;

    USER_TYPES(int id, String description) {
        this.id = id;
        this.description = description;
    }

    public int getId() {
        return id;
    }

    public void setId(int id) {
        this.id = id;
    }

    public String getDescription() {
        return description;
    }

    public void setDescription(String description) {
        this.description = description;
    }
}
