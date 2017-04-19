import React, { Component } from 'react';
import Dropzone from 'react-dropzone';
import { Link } from 'react-router-dom';
import Tooltip from 'react-tooltip-component';
import { connect } from 'react-redux';

import CheckboxInput from '../form/CheckboxInput';
import TextInput from '../form/TextInput';
import SVG from '../SVG';
import sendMailingList from '../../AC/mailing';


class UploadMailList extends Component {
  constructor() {
    super();

    this.state = {
      name: '',
      preview: '',
      'addresses-manually': false,
      'mail-type': 'first-class',
      'mail-product': 'postcard',
      'mail-validity': '1week'
    };

    this.handleChange = this.handleChange.bind(this);
    this.onDrop = this.onDrop.bind(this);
    this.removeFile = this.removeFile.bind(this);
  }


  handleChange(input, value) {
    this.setState({
      [input]: value
    });
  }

  onDrop(acceptedFile) {
    const file = acceptedFile[0];
    const { name, preview } = file;
    this.setState({
      preview,
      name
    });
  }

  removeFile() {
    this.setState({
      name: '',
      preview: ''
    });
  }

  render() {
    const { mailing, sendMailingList: send } = this.props;
    const { isLoading } = mailing;

    const dropzoneContent = this.state.preview
      ? <div>
          <span onClick={this.removeFile} href="#" className="drop-zone__btn close">
            <SVG name="cross" className="icon-cross" />
          </span>
          <SVG name="csv" className="icon-csv" />
          <p className="drop-zone__file-name">{this.state.name}</p>
        </div>
      : <div>
          <SVG name="draganddrop" className="icon-drop" />
          <p className="font-text">Drag and drop your .csv file here or click anywhere to upload</p>
        </div>;

    const dropZoneClass = this.state.preview
      ? 'drop-zone drop-zone--dropped'
      : 'drop-zone drop-zone--dropping';

    return (
      <div className="upload-mail">
        <div className="upload-mail__drop-zone">
          <Tooltip title="Do you need a help with a bulk upload?" position="left">
            <span href="#" className="drop-zone__btn question">
              <SVG name="question-mark" className="icon-question" />
            </span>
          </Tooltip>

          <Dropzone disableClick={!!this.state.preview} className={dropZoneClass} accept="text/csv" multiple={false} onDrop={this.onDrop}>
            <div className="drop-zone__content">
              { dropzoneContent }
            </div>
          </Dropzone>

          <div className="drop-zone__offer">
            <span>or</span>
            <p>Skip this field to insert the addresses manually within the next step.</p>
          </div>
        </div>

        <div className="upload-mail__row">
          <h2>Mail type</h2>
          <p>First class guarantees next working day delivery, standart class usually takes 3-5 days</p>

          <div className="row">
            <div className="col-lg-4 col-xl-3">
              <CheckboxInput
                id="mail-type-first-class"
                label="First Class"
                type="radio"
                name="mail-type"
                value="first-class"
                onChange={(event) => {
                  this.handleChange(event.target.name, event.target.value);
                }}
                defaultChecked
              />
            </div>

            <div className="col-lg-4 col-xl-3">
              <CheckboxInput
                id="mail-type-unsorted"
                label="Standart - Unsorted"
                type="radio"
                name="mail-type"
                value="unsorted"
                onChange={(event) => {
                  this.handleChange(event.target.name, event.target.value);
                }}
              />
            </div>

            <div className="col-lg-4 col-xl-3">
              <CheckboxInput
                id="mail-type-sorted"
                label="Standart - Sorted"
                type="radio"
                name="mail-type"
                value="sorted"
                onChange={(event) => {
                  this.handleChange(event.target.name, event.target.value);
                }}
              />
            </div>
          </div>
        </div>

        <div className="upload-mail__row">
          <h2>Product</h2>
          <p>Specify what kind of mailing are you going to send</p>

          <div className="row">
            <div className="col-lg-4 col-xl-3">
              <CheckboxInput
                id="product-postcard"
                label="Post Card"
                type="radio"
                name="mail-product"
                value="postcard"
                defaultChecked
                onChange={(event) => {
                  this.handleChange(event.target.name, event.target.value);
                }}
              />
            </div>

            <div className="col-lg-4 col-xl-3">
              <CheckboxInput
                id="product-letter"
                label="Letter"
                type="radio"
                name="mail-product"
                value="letter"
                onChange={(event) => {
                  this.handleChange(event.target.name, event.target.value);
                }}
              />
            </div>

            <div className="col-lg-4 col-xl-3">
              <CheckboxInput
                id="product-self-mailer"
                label="Self-mailer"
                type="radio"
                name="mail-product"
                value="self"
                onChange={(event) => {
                  this.handleChange(event.target.name, event.target.value);
                }}
              />
            </div>
          </div>
        </div>

        <div className="upload-mail__row">
          <h2>Validity</h2>
          <p>Tell us when the mailing list will expire</p>

          <div className="row">
            <div className="col-lg-4 col-xl-3">
              <CheckboxInput
                id="validity-1week"
                label="1 week"
                type="radio"
                name="mail-validity"
                value="1week"
                defaultChecked
                onChange={(event) => {
                  this.handleChange(event.target.name, event.target.value);
                }}
              />
            </div>

            <div className="col-lg-4 col-xl-3">
              <CheckboxInput
                id="validity-90days"
                label="90 days"
                type="radio"
                name="mail-validity"
                value="90days"
                onChange={(event) => {
                  this.handleChange(event.target.name, event.target.value);
                }}
              />
            </div>

            <div className="col-lg-4 col-xl-3">
              <CheckboxInput
                id="validity-unlimited"
                label="Unlimited"
                type="radio"
                name="mail-validity"
                value="unlimited"
                onChange={(event) => {
                  this.handleChange(event.target.name, event.target.value);
                }}
              />
            </div>
          </div>
        </div>

        <div className="upload-mail__row">
          <h2>Name</h2>
          <p>Give it a nice name for future reference</p>

          <div className="row">
            <div className="col-lg-5 col-xl-3">
              <TextInput
                placeholder="Name"
                name="name"
                value={this.state.name}
                onChange={(event) => {
                  this.handleChange(event.target.name, event.target.value);
                }}
              />
            </div>
          </div>

        </div>

        <button type="button"
                className="btn-main"
                disabled={isLoading}
                onClick={(e) => send(this.state)}
        >Create mailing list</button>


        <br />
        <Link to="/map-columns.html">Create mailing list</Link>

      </div>
    );
  }
}

export default connect(
  (state) => {
    const { mailing } = state;

    return { mailing };
  }, {
    sendMailingList
  })
(UploadMailList);
