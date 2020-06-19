package com.talker.api.utils;

import java.security.MessageDigest;
import java.security.NoSuchAlgorithmException;

public class PasswordHasher {

    private static String PASSWORD_SALT = "'AL<muj2c|vKKhZSFfVMX=iodbvf]q]aKJ`-aNa~|+m@]9tK)8Ac+_lJ0s8-^2}> ');";;

    public static String hash(String password) {
        try {
            MessageDigest sha = MessageDigest.getInstance("SHA-512");
            String salt = PASSWORD_SALT;
            String passWithSalt = password + salt;
            byte[] passBytes = passWithSalt.getBytes();
            byte[] passHash = sha.digest(passBytes);
            StringBuilder sb = new StringBuilder();
            for(int i=0; i< passHash.length ;i++) {
                sb.append(Integer.toString((passHash[i] & 0xff) + 0x100, 16).substring(1));
            }
            String generatedPassword = sb.toString();
            System.out.println();
            return generatedPassword;
        } catch (NoSuchAlgorithmException e) { e.printStackTrace(); }
        return null;
    }

}
