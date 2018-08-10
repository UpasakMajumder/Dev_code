import React from 'react';
import PropTypes from 'prop-types';
import DateInput from 'react-datepicker';
import moment from 'moment';
import SVG from 'app.dump/SVG';
/* helpers */
import { dateFormat } from 'app.helpers/time';
/* styles */
import 'react-datepicker/dist/react-datepicker.css';

const getLabel = label => (label ? <span class="input__label">{label}</span> : null);

const generateInputClass = (inputClass, error) => {
  const classNames = ['input__text'];
  if (inputClass) classNames.push[inputClass];
  return classNames.join(' ');
};

const Datepicker = ({
  selected,
  selectsStart,
  selectsEnd,
  startDate,
  endDate,
  onChange,
  dateFormat,
  label,
  placeholder,
  inputClass,
  readOnly,
  minDate,
  onBlur,
  error
}) => {
  const errorElement = error ? (
    [<div key="0" className="input--error" />, <span key="1" className="input__error input__error--noborder">{error}</span>]
  ) : null;

  return (
    <div className="input__wrapper input__wrapper--with-icon">
      {getLabel(label)}
      <DateInput
        className={generateInputClass(inputClass, error)}
        placeholder={placeholder}
        selected={selected}
        selectsStart={selectsStart}
        startDate={startDate}
        selectsEnd={selectsEnd}
        endDate={endDate}
        onChange={onChange} // argument is momentum object date
        dateFormat={dateFormat}
        readOnly={readOnly}
        minDate={minDate}
        onBlur={onBlur}
      />
      <SVG
        name="calendar_1"
        className="icon-input"
      />
      {errorElement}
    </div>
  );
};

Datepicker.defaultProps = {
  selectsStart: false,
  selectsEnd: false,
  startDate: null,
  endDate: null,
  dateFormat,
  label: '',
  placeholder: '',
  inputClass: '',
  readOnly: false,
  minDate: null,
  error: ''
};

Datepicker.propTypes = {
  selected: PropTypes.object,
  selectsStart: PropTypes.bool,
  selectsEnd: PropTypes.bool,
  startDate: PropTypes.object,
  endDate: PropTypes.object,
  onChange: PropTypes.func.isRequired,
  dateFormat: PropTypes.string,
  label: PropTypes.string,
  placeholder: PropTypes.string,
  inputClass: PropTypes.string,
  readOnly: PropTypes.bool,
  minDate: PropTypes.object,
  onBlur: PropTypes.func,
  error: PropTypes.string
};

export default Datepicker;
