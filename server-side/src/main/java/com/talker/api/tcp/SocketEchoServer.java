package com.talker.api.tcp;

import com.talker.api.dao.UserDao;
import com.talker.api.entity.UserEntity;
import com.talker.api.enums.STATUS_UPDATE_RESPONSE;
import com.talker.api.service.ContactsService;
import org.apache.logging.log4j.LogManager;
import org.apache.logging.log4j.Logger;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.beans.factory.annotation.Value;
import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Configuration;
import org.springframework.integration.annotation.*;
import org.springframework.integration.channel.DirectChannel;
import org.springframework.integration.config.EnableIntegration;
import org.springframework.integration.ip.tcp.TcpInboundGateway;
import org.springframework.integration.ip.tcp.TcpOutboundGateway;
import org.springframework.integration.ip.tcp.connection.AbstractClientConnectionFactory;
import org.springframework.integration.ip.tcp.connection.AbstractServerConnectionFactory;
import org.springframework.integration.ip.tcp.connection.TcpNetClientConnectionFactory;
import org.springframework.integration.ip.tcp.connection.TcpNetServerConnectionFactory;
import org.springframework.messaging.MessageChannel;
import org.springframework.messaging.MessageHandler;
import org.springframework.scheduling.annotation.Scheduled;

import javax.annotation.PostConstruct;
import java.sql.Timestamp;
import java.util.HashMap;
import java.util.Map;


@EnableIntegration
@IntegrationComponentScan
@Configuration
public class SocketEchoServer {

    @Value("${tcp.server.port}")
    private int port;

    @MessagingGateway(defaultRequestChannel="toTcp")
    public interface Gateway {

        String viaTcp(String in);

    }

    @Bean
    @ServiceActivator(inputChannel="toTcp")
    public MessageHandler tcpOutGate(AbstractClientConnectionFactory connectionFactory) {
        TcpOutboundGateway gate = new TcpOutboundGateway();
        gate.setConnectionFactory(connectionFactory);
        gate.setOutputChannelName("resultToString");
        return gate;
    }

    @Bean
    public TcpInboundGateway tcpInGate(AbstractServerConnectionFactory connectionFactory)  {
        TcpInboundGateway inGate = new TcpInboundGateway();
        inGate.setConnectionFactory(connectionFactory);
        inGate.setRequestChannel(fromTcp());
        return inGate;
    }

    @Bean
    public MessageChannel fromTcp() {
        return new DirectChannel();
    }

    @MessageEndpoint
    public static class Echo {

        private final Logger log = LogManager.getLogger(Echo.class);

        @Autowired
        private UserDao userDao;


        @Autowired
        private ContactsService contactsService;


        Map<Integer, UserUpdater> connectedUsers;

        @PostConstruct
        public void init(){
            connectedUsers = new HashMap<>();
        }

        @Scheduled(fixedRate = 10000)
        public void clearConnectedUsersList(){

            Timestamp timestamp = new Timestamp(System.currentTimeMillis());;

            //log.info("USER SCHEDULER RUN...");

            for (Map.Entry<Integer, UserUpdater> e : connectedUsers.entrySet()) {

                //log.info("diffrence: "+ (timestamp.getTime() - e.getValue().lastUpdate.getTime()));

                if(timestamp.getTime() - e.getValue().lastUpdate.getTime() > 20000){
                    log.info("USER ID = "+ e.getKey()+" IS IDLE - REMOVING FORM ACTIVE USERS LIST");

                    connectedUsers.remove(e.getKey());
                }

                //System.out.println(entry.getKey() + "/" + entry.getValue());
            }

        }

        @Transformer(inputChannel="fromTcp", outputChannel="toEcho")
        public String convert(byte[] bytes) {
            return new String(bytes);
        }

        @ServiceActivator(inputChannel="toEcho")
        public String receiveHeartbeatMessage(String messageStr) {

            //log.info("Status update received: "+ messageStr);

            if (messageStr.contains("status")) {
                //log.info("Status update received");

                String[] messageStrSplited=messageStr.split(";");

                Integer userId = Integer.valueOf(messageStrSplited[1]);
                //Timestamp clientTimestrap= Timestamp.valueOf(ss[2]);

                Timestamp timestamp = new Timestamp(System.currentTimeMillis());

                UserUpdater uu;
                if(connectedUsers.containsKey(userId)){
                    uu = connectedUsers.get(userId);

                    //log.info("USER ID = "+ userId+" REFRESHED");
                }else{
                    log.info("USER ID = "+ userId+" ADDED TO ACTIVE USERS LIST");

                    UserEntity ue = userDao.findById(userId).get();

                    uu = new UserUpdater(timestamp, ue, contactsService);

                    connectedUsers.put(userId, uu);
                }

                STATUS_UPDATE_RESPONSE response = uu.update(timestamp);

               // log.info("RESPONSE: "+ response);

                return response.getResponseValue();
            } else {
                log.error("Unexpected message content from client: " + messageStr);

                return "Unknown request";
            }
        }

        @Transformer(inputChannel="resultToString")
        public String convertResult(byte[] bytes) {
            return new String(bytes);
        }

    }

    @Bean
    public AbstractClientConnectionFactory clientCF() {
        return new TcpNetClientConnectionFactory("localhost", this.port);
    }

    @Bean
    public AbstractServerConnectionFactory serverCF() {
        return new TcpNetServerConnectionFactory(this.port);
    }

}
