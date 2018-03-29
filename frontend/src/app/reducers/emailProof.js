import { EMAIL_PROOF_TOGGLE } from 'app.consts';

const defaultState = {
  show: false,
  emailProofUrl: ''
};

export default (state = defaultState, action) => {
  const { type, payload } = action;
  switch (type) {
  case EMAIL_PROOF_TOGGLE:
    return {
      show: payload.show,
      emailProofUrl: payload.emailProofUrl
    };

  default:
    return state;
  }
};
