package com.talker.api.ws;

import com.talker.api.dto.StatusReponse;
import com.talker.api.exceptions.AuthenticationFailure;
import com.talker.api.config.CurrentUser;
import com.talker.api.dto.requests.UserRequest;
import com.talker.api.dto.UserResponse;
import com.talker.api.entity.UserEntity;
import com.talker.api.service.ContactsService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;

import javax.servlet.http.HttpServletRequest;
import javax.validation.Valid;
import java.util.List;

@RestController
@CurrentUser
@RequestMapping("/api/contacts/")
public class ContactsController extends UserAuthorized {

	@Autowired
	private ContactsService contactsService;


	@PostMapping("list")
	public List<UserResponse> contactsList(HttpServletRequest request) throws AuthenticationFailure {
		UserEntity user = getUserFromRequest(request);
		return contactsService.getUserAllContacts(user);
	}

	@PostMapping("add")
	public StatusReponse addContact(@Valid @RequestBody UserRequest userRequest, HttpServletRequest request) throws AuthenticationFailure {

		UserEntity user = getUserFromRequest(request);
		contactsService.addContact(user, userRequest);


		StatusReponse sr  = new StatusReponse();
		sr.setError("User added successfully");
		sr.setMessage("The user has been successfully added <br />to the contact list.");


		return sr;
	}

	@PostMapping("remove")
	public StatusReponse removeContact(@Valid @RequestBody UserRequest userRequest, HttpServletRequest request) throws AuthenticationFailure {

		UserEntity user = getUserFromRequest(request);
		contactsService.removeContact(user, userRequest);


		StatusReponse sr  = new StatusReponse();
		sr.setError("User removed successfully");
		sr.setMessage("The user has been successfully removed <br />form your contact list.");

		return sr;
	}


}
