import React, { Component } from 'react';
import CheckboxInput from '../form/CheckboxInput';
import TextInput from '../form/TextInput';
import Dropzone from 'react-dropzone';
import { Link } from 'react-router-dom';


class UploadMailList extends Component {
  constructor() {
    super();

    this.state = {
      inputs: {
        file: 1,
        'addresses-manually': false,
        'mail-type': 'first-class',
        'mail-product': 'postcard',
        'mail-validity': '1week',
        name: ''
      }
    };

    this.handleChange = this.handleChange.bind(this);
    this.onDrop = this.onDrop.bind(this);
  }


  handleChange(input, value) {
    this.setState({
      inputs: {
        ...this.state.inputs,
        [input]: value
      }
    });
  }

  onDrop(acceptedFile) {
    this.setState({
      inputs: {
        ...this.state.inputs,
        file: acceptedFile
      }
    });
  }


  render() {
    return (
      <div>
        <div className="upload-mail__drop-zone">
          <Dropzone accept="text/csv" multiple={false} onDrop={this.onDrop}>
            <div>Try dropping some files, or click to select files to upload.</div>
          </Dropzone>

          <CheckboxInput
            id="checkbox-drop-zone"
            label="I will insert addresses manually in the next step"
            type="checkbox"
            name="addresses-manually"
            value="1"
            onChange={(event) => { this.handleChange(event.target.name, event.target.checked); }}
          />
        </div>

        <div className="upload-mail__mail-type">
          <CheckboxInput
            id="mail-type-first-class"
            label="First Class"
            type="radio"
            name="mail-type"
            value="first-class"
            onChange={(event) => { this.handleChange(event.target.name, event.target.value); }}
            defaultChecked
          />

          <CheckboxInput
            id="mail-type-unsorted"
            label="Standart - Unsorted"
            type="radio"
            name="mail-type"
            value="unsorted"
            onChange={(event) => { this.handleChange(event.target.name, event.target.value); }}
          />

          <CheckboxInput
            id="mail-type-sorted"
            label="Standart - Sorted"
            type="radio"
            name="mail-type"
            value="sorted"
            onChange={(event) => { this.handleChange(event.target.name, event.target.value); }}
          />

        </div>

        <div className="upload-mail__product">
          <CheckboxInput
            id="product-postcard"
            label="Post Card"
            type="radio"
            name="mail-product"
            value="postcard"
            defaultChecked
            onChange={(event) => { this.handleChange(event.target.name, event.target.value); }}
          />

          <CheckboxInput
            id="product-letter"
            label="Letter"
            type="radio"
            name="mail-product"
            value="letter"
            onChange={(event) => { this.handleChange(event.target.name, event.target.value); }}
          />

          <CheckboxInput
            id="product-self-mailer"
            label="Self-mailer"
            type="radio"
            name="mail-product"
            value="self"
            onChange={(event) => { this.handleChange(event.target.name, event.target.value); }}
          />
        </div>

        <div className="upload-mail__validity">
          <CheckboxInput
            id="validity-1week"
            label="1 week"
            type="radio"
            name="mail-validity"
            value="1week"
            defaultChecked
            onChange={(event) => { this.handleChange(event.target.name, event.target.value); }}
          />

          <CheckboxInput
            id="validity-90days"
            label="90 days"
            type="radio"
            name="mail-validity"
            value="90days"
            onChange={(event) => { this.handleChange(event.target.name, event.target.value); }}
          />

          <CheckboxInput
            id="validity-unlimited"
            label="Unlimited"
            type="radio"
            name="mail-validity"
            value="unlimited"
            onChange={(event) => { this.handleChange(event.target.name, event.target.value); }}
          />
        </div>

        <div className="upload-mail__name">
          <TextInput
            placeholder="Name"
            name="name"
            onChange={(event) => { this.handleChange(event.target.name, event.target.value); }}
          />
        </div>

        <Link to="/map-columns.html">Create mailing list</Link>

      </div>
    );
  }
}

export default UploadMailList;
