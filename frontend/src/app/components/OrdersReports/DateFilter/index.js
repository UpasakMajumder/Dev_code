import React, { Component } from 'react';
import PropTypes from 'prop-types';
import moment from 'moment';
/* component */
import Button from 'app.dump/Button';
import Datepicker from 'app.dump/Form/Datepicker';

class DateFilter extends Component {
  state = {
    dateFrom: this.props.dateFrom,
    dateTo: this.props.dateTo
  }

  static propTypes = {
    ui: PropTypes.shape({
      title: PropTypes.string.isRequired,
      labelNow: PropTypes.string.isRequired,
      labelApply: PropTypes.string.isRequired
    }).isRequired,
    dateFrom: PropTypes.object,
    dateTo: PropTypes.object,
    applyDate: PropTypes.func.isRequired
  }

  handleChangeDate = (value, field, event) => {
    if (event) event.preventDefault();
    this.setState({
      [field]: value
    });
  };

  handleApplyDate = () => {
    this.props.applyDate(this.state.dateTo, this.state.dateFrom);
  };

  render() {
    const {
      ui: {
        title,
        labelNow,
        labelApply
      }
    } = this.props;

    const {
      dateFrom,
      dateTo
    } = this.state;

    return (
      <div>
        <h2 className="mb-3">{title}</h2>
        <div className="mb-3">
          <div className="flex--center--between mb-3">
            <Datepicker
              selected={dateFrom}
              selectsStart
              startDate={dateFrom}
              endDate={dateTo}
              onChange={(date) => { this.handleChangeDate(date, 'dateFrom'); }}
            />
            <a href="#" className="ml-2 link" onClick={event => this.handleChangeDate(moment(), 'dateFrom', event)}>{labelNow}</a>
          </div>

          <div className="flex--center--between">
            <Datepicker
              selected={dateTo}
              selectsEnd
              startDate={dateFrom}
              endDate={dateTo}
              onChange={(date) => { this.handleChangeDate(date, 'dateTo'); }}
            />
            <a href="#" className="ml-2 link" onClick={event => this.handleChangeDate(moment(), 'dateTo', event)}>{labelNow}</a>
          </div>
        </div>
        <div>
          <Button
            text={labelApply}
            type="action"
            onClick={this.handleApplyDate}
          />
        </div>
      </div>
    );
  }
}

export default DateFilter;
