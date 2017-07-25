import React, { Component } from 'react';
import { Link } from 'react-router-dom';

import Steps from '../Steps';
import Breadcrumbs from '../Breadcrumbs';

class MapColumns extends Component {
  constructor() {
    super();

    this.data = [
      {
        title: 'Title',
        optional: true,
        data: ['1', '2', '3']
      },
      {
        title: 'Name',
        optional: false,
        data: ['1', '2', '3']
      },
      {
        title: 'Second Name',
        optional: true,
        data: ['1', '2', '3']
      },
      {
        title: 'First address line',
        optional: false,
        data: ['1', '2', '3']
      },
      {
        title: 'Second address line',
        optional: false,
        data: ['1', '2', '3']
      },
      {
        title: 'City',
        optional: false,
        data: ['1', '2', '3']
      },
      {
        title: 'State',
        optional: false,
        data: ['1', '2', '3']
      },
      {
        title: 'ZIP Code',
        optional: false,
        data: ['1', '2', '3']
      }
    ];

    this.state = {
      inputs: {}
    };

    this.steps = [
      'Add a mailing list',
      'Map columns',
      'Wait for a proofing'
    ];

    this.crumbs = [
      { title: 'Home', link: '/' },
      { title: 'Mailing list', link: '/mailing-list.html' },
      { title: 'New mailing list', link: '#' }
    ];

    this.handleChange = this.handleChange.bind(this);
  }

  componentDidMount() {
    this.setState((prevState) => {
      return {
        inputs: {
          ...prevState.inputs,
          ...this.inputsState
        }
      };
    });
  }

  handleChange(field, value) {
    this.setState((prevState) => {
      return {
        inputs: {
          ...prevState.inputs,
          [field]: value
        }
      };
    });
  }


  render() {
    this.inputsState = {};

    const dataRows = this.data.map((item) => {
      this.inputsState[item.title] = item.optional ? '' : item.data[0];

      const selectList = item.data.map((option) => {
        return <option key={option}>{option}</option>;
      });

      const key = item.title.split(' ').join('');

      return (
        <div key={key} className="input__wrapper">
          <span className="input__label">{item.title}</span>
          <span className="input__right-label">{item.optional ? 'Optional' : ''}</span>

          <div className="input__select">
            <select>
              { selectList }
            </select>
          </div>
        </div>
      );
    });

    return (
      <div className="content">
        <div className="content__inner">
          <div className="content-header">
            <h1 className="content-header__page-name">Map columns</h1>
            <Breadcrumbs crumbs={this.crumbs} />
          </div>
          <div className="content-block map-columns">
            <Steps current={1} steps={this.steps}/>
            <div className="map-columns__paragraphs">
              <p className="p-info"> We tried to map the items in your uploaded files into the correct address fields.</p>
              <p className="p-info">Please check if we got it right and correct any mistakes we might have made.</p>
            </div>
            <p className="p-note">
              Note: This is just for the address. You will be able to use other fields in the templating process.
            </p>

            <div className="map-columns__form map-columns-form">
              <div className="map-columns-form__group-wrapper">
              { dataRows }
              </div>
            </div>

            <div className="btn-group btn-group--left">
              <button type="button" className="btn-action btn-action--secondary">Reupload list</button>
              <button type="button" className="btn-action">Process my list</button>
            </div>

            <Link to="/new-mailing.html">Reload list</Link>
            <Link to="/mail-processing.html">Process my list</Link>
          </div>
        </div>
      </div>
    );
  }
}

export default MapColumns;
