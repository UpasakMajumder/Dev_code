import React, { Component } from 'react';
import { Link } from 'react-router-dom';

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
        <div key={key} className="row">
          <div className="col-5">
            <p>{item.title}</p>
          </div>

          <div className="col-4">
            <div className="input__select">
              <select onChange={ (event) => { this.handleChange(item.title, event.target.value); }}>
                { selectList }
              </select>
            </div>
          </div>

          {item.optional ? <div className="col-3"><p>Optional</p></div> : null}
        </div>
      );
    });

    return (
      <div>
        { dataRows }
        <Link to="/new-mailing.html">Reload list</Link>
        <Link to="/mail-processing.html">Process my list</Link>
      </div>
    );
  }
}

export default MapColumns;
