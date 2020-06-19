package com.talker.api.config;

import com.talker.api.entity.UserEntity;
import io.jsonwebtoken.Claims;

import io.jsonwebtoken.Jwts;

import io.jsonwebtoken.SignatureAlgorithm;
import org.springframework.beans.factory.annotation.Value;
import org.springframework.cglib.core.internal.Function;
import org.springframework.stereotype.Component;

import javax.servlet.http.HttpServletRequest;
import java.io.Serializable;
import java.util.Date;
import java.util.HashMap;
import java.util.Map;


@Component
public class JwtTokenUtil implements Serializable {

    private static final long serialVersionUID = -2550185165626007488L;

    public static final long JWT_TOKEN_VALIDITY =  24 * 60 * 60    *  365   *    5;

    @Value("${jwt.secret}")
    private String secret;

    public String getUsernameFromRequest(HttpServletRequest request) {
        return getClaimFromToken(JwtFilter.getJwtFromRequest(request), Claims::getSubject);
    }

    public int getUserIdFromRequest(HttpServletRequest request) {
        return (int) getAllClaimsFromToken(JwtFilter.getJwtFromRequest(request)).get("user-id");
    }

    public String getUserStaticTokenFromRequest(HttpServletRequest request) {
        return (String) getAllClaimsFromToken(JwtFilter.getJwtFromRequest(request)).get("user-token");
    }

    public String getUserClientIpFromRequest(HttpServletRequest request) {
        return (String) getAllClaimsFromToken(JwtFilter.getJwtFromRequest(request)).get("client-ip");
    }


    public Date getExpirationDateFromToken(String token) {
        return getClaimFromToken(token, Claims::getExpiration);
    }

    public <T> T getClaimFromToken(String token, Function<Claims, T> claimsResolver) {
        final Claims claims = getAllClaimsFromToken(token);
        return claimsResolver.apply(claims);
    }

    //for retrieveing any information from token we will need the secret key
    public Claims getAllClaimsFromToken(String token) {
        return Jwts.parser().setSigningKey(secret).parseClaimsJws(token).getBody();
    }

    //check if the token has expired
    private Boolean isTokenExpired(String token) {
        final Date expiration = getExpirationDateFromToken(token);
        return expiration.before(new Date());
    }


    public String generateToken(UserEntity userDetails, HttpServletRequest request) {
        Map<String, Object> claims = new HashMap<>();

        claims.put("user-id", userDetails.getId());
        claims.put("user-token", userDetails.getToken());
        claims.put("client-ip", getIpAddress(request));

        return doGenerateToken(claims, userDetails);
    }

    private String doGenerateToken(Map<String, Object> claims, UserEntity userDetails) {

        return Jwts.builder().setClaims(claims)
                .setSubject(userDetails.getUsername())
                .setIssuedAt(new Date(System.currentTimeMillis()))
                .setExpiration(new Date(System.currentTimeMillis() + JWT_TOKEN_VALIDITY * 1000))
                .signWith(SignatureAlgorithm.HS512, secret).compact();
    }


    public boolean validateRequest(HttpServletRequest request){
        String token  = JwtFilter.getJwtFromRequest(request);
//        System.out.println("token ip: "+ getUserClientIpFromRequest(request));
//        System.out.println("request ip: "+ getIpAddress(request));
        return !isTokenExpired(token);// &&
                //getUserClientIpFromRequest(request).equals(getIpAddress(request));
    }

    public String getIpAddress(HttpServletRequest request){
        String ipAddress = request.getHeader("X-FORWARDED-FOR");
        if (ipAddress == null) {
            ipAddress = request.getRemoteAddr();
        }
        return ipAddress;
    }


//    public Boolean compareUserToRequest(HttpServletRequest request, UserEntity userDetails) {
//        String token  = JwtFilter.getJwtFromRequest(request);
//
//        TokenBody tokenBody = ((TokenBody) Jwts.parser().setSigningKey(secret).parseClaimsJws(token).getBody().get("client-data"));
//        UserEntity user = tokenBody.getUserEntity();
//
//        return (user.getUsername().equals(userDetails.getUsername()) &&
//                user.getPassword().equals(userDetails.getPassword()) &&
//                user.getToken().equals(userDetails.getToken()) &&
//                request.getRemoteAddr().equals(tokenBody.getClientIp()) &&
//                !isTokenExpired(token));
//    }




}
