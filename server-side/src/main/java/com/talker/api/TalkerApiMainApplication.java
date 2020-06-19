package com.talker.api;

import com.talker.api.config.JwtFilter;
import org.springframework.boot.SpringApplication;
import org.springframework.boot.autoconfigure.SpringBootApplication;
import org.springframework.boot.web.servlet.FilterRegistrationBean;
import org.springframework.context.annotation.Bean;
import org.springframework.scheduling.annotation.EnableScheduling;

@SpringBootApplication
@EnableScheduling
public class TalkerApiMainApplication {

	@Bean
	public FilterRegistrationBean<JwtFilter> jwtFilter() {
		final FilterRegistrationBean<JwtFilter> registrationBean = new FilterRegistrationBean<>();
		registrationBean.setFilter(new JwtFilter());
		registrationBean.addUrlPatterns("/secure/*");
		registrationBean.addUrlPatterns("/api/*");

		return registrationBean;
	}


	public static void main(String[] args) {

		SpringApplication.run(TalkerApiMainApplication.class, args);

		/*TcpServerStarter tcpServerStarter = new TcpServerStarter();
		tcpServerStarter.startTcpSever();*/

	}

}
