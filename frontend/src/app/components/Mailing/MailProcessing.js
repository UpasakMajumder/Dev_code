import React from 'react';
import { Link } from 'react-router-dom';

import Steps from '../Steps';
import Breadcrumbs from '../Breadcrumbs';
import SVG from '../SVG';

const MailProcessing = () => {
  const crumbs = [
    { title: 'Home', link: '/' },
    { title: 'Mailing list', link: '/mailing-list.html' },
    { title: 'New mailing list', link: '#' }
  ];

  const steps = [
    'Add a mailing list',
    'Map columns',
    'Wait for a proofing'
  ];


  return (
  <div className="content">
    <div className="content__inner">
      <div className="content-header">
        <h1 className="content-header__page-name">We are now processing your list...</h1>
        <Breadcrumbs crumbs={crumbs} />
      </div>
      <div className="content-block mail-processing">
        <Steps current={2} steps={steps}/>

        <SVG name="envelope" className="mail-processing__central-img" />

        <div className="mail-processing__text-group mail-processing__text-group--centered">
          <p className="p-info">Your address list was sent to processing. This will take about 5-10 minutes.</p>
          <p className="p-info">You can continue working, we will keep you informed once your list is ready to use.</p>
        </div>

        <div className="btn-group btn-group--centered">
          <button type="button" className="btn-action btn-action--secondary">Create new mailing list</button>
          <button type="button" className="btn-action">See products</button>
        </div>

        <Link to="/new-mailing.html">Create new mailing list</Link>
        <a href="#">See products</a>

      </div>
    </div>
  </div>
  );
};

export default MailProcessing;
