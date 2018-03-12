import React from 'react';
import PropTypes from 'prop-types';
import moment from 'moment';
/* component */
import Button from 'app.dump/Button';
import Datepicker from 'app.dump/Form/Datepicker';

const DateFilter = ({
  ui: {
    title,
    labelNow,
    labelApply
  },
  dateFrom,
  dateTo,
  changeDate,
  applyDate
}) => {
  const setNow = (e, field) => {
    e.preventDefault();
    changeDate(moment(), field);
  };

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
            onChange={(date) => { changeDate(date, 'dateFrom'); }}
          />
          <a href="#" className="ml-2 link" onClick={e => setNow(e, 'dateFrom')}>{labelNow}</a>
        </div>

        <div className="flex--center--between">
          <Datepicker
            selected={dateTo}
            selectsEnd
            startDate={dateFrom}
            endDate={dateTo}
            onChange={(date) => { changeDate(date, 'dateTo'); }}
          />
          <a href="#" className="link" onClick={e => setNow(e, 'dateTo')}>{labelNow}</a>
        </div>
      </div>
      <div>
        <Button
          text={labelApply}
          type="action"
          onClick={applyDate}
        />
      </div>
    </div>
  );
};

DateFilter.propTypes = {
  ui: PropTypes.shape({
    title: PropTypes.string.isRequired,
    labelNow: PropTypes.string.isRequired,
    labelApply: PropTypes.string.isRequired
  }).isRequired,
  dateFrom: PropTypes.object,
  dateTo: PropTypes.object,
  changeDate: PropTypes.func.isRequired,
  applyDate: PropTypes.func.isRequired
};

export default DateFilter;
