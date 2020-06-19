package com.talker.api.service;

import com.talker.api.dao.DescriptionsDao;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

@Service
@Transactional
public class DescriptionsService {

    @Autowired
    public DescriptionsDao descriptionsDao;

}
