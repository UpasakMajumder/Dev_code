import React from 'react';
import PropTypes from 'prop-types';
import DatePicker from 'react-datepicker';
import moment from 'moment';
/* styles */
import 'react-datepicker/dist/react-datepicker.css';
/* helpers */
import { dateFormat } from 'app.helpers/time';

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
  const setNow = field => changeDate(moment(), field);

  return (
    <div>
      <p>{title}</p>
      <div>
        <div>
          <DatePicker
            selected={dateFrom}
            selectsStart
            startDate={dateFrom}
            endDate={dateTo}
            onChange={(date) => { changeDate(date, 'dateFrom'); }}
            dateFormat={dateFormat}
          />
          <button type="button" onClick={() => setNow('dateFrom')}>{labelNow}</button>
        </div>

        <div>
          <DatePicker
            selected={dateTo}
            selectsEnd
            startDate={dateFrom}
            endDate={dateTo}
            onChange={(date) => { changeDate(date, 'dateTo'); }}
            dateFormat={dateFormat}
          />
          <button type="button" onClick={() => setNow('dateTo')}>{labelNow}</button>
        </div>
      </div>
      <div>
        <button type="button" onClick={applyDate}>{labelApply}</button>
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
