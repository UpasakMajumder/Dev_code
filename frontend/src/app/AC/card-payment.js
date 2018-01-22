import axios from 'axios';
import { SUBMIT_CARD, GET_SUBMISSIONID, FETCH, FAILURE, SUCCESS } from 'app.consts';
import { CARD_PAYMENT } from 'app.globals';

const redirectUser = (RedirectURL, SubmissionID) => {
  axios({
    method: 'get',
    url: `${RedirectURL}/${SubmissionID}`
  }).then((response) => {
    const { data: redirectData } = response;
    if (!redirectData.success) {
      dispatch({
        type: SUBMIT_CARD + FAILURE,
        alert: redirectData.errorMessage
      });
    } else {
      const { redirectionURL } = redirectData.payload;

      dispatch({ type: SUBMIT_CARD + SUCCESS });

      if (redirectionURL) {
        location.assign(redirectionURL);
      } else {
        setTimeout(() => redirectUser(RedirectURL, SubmissionID), 500);
      }
    }
  }).catch(() => {
    dispatch({ type: SUBMIT_CARD + FAILURE });
  });
};

export default (fields, cardType) => {
  const { name, cvc, number, expiry } = fields;

  return async (dispatch) => {
    try {
      dispatch({ type: GET_SUBMISSIONID + FETCH });

      const { URL3DSi, ResultURL, RedirectURL, SubmissionIDURL,
        CustomerIdentifier_MerchantCode,
        CustomerIdentifier_LocationCode,
        CustomerIdentifier_CustomerCode,
        TerminalIdentifier_LocationCode,
        TerminalIdentifier_TerminalCode,
        TerminalIdentifier_MerchantCode,
        APCount, PTCount
      } = CARD_PAYMENT;

      const { data: submissionIDData } = await axios({ method: 'get', url: SubmissionIDURL });

      if (!submissionIDData.success) {
        dispatch({
          type: GET_SUBMISSIONID + FAILURE,
          alert: submissionIDData.errorMessage
        });
      } else {
        dispatch({ type: GET_SUBMISSIONID + SUCCESS });

        const { submissionID: SubmissionID } = submissionIDData.payload;

        const data = {
          CreditCard_CSCValue: cvc,
          CreditCard_ExpirationMonth: expiry.substr(0, 2),
          CreditCard_ExpirationYear: expiry.substr(2),
          CreditCard_CardType: cardType,
          CreditCard_NameOnCard: name,
          CreditCard_CardAccountNumber: number,
          APCount,
          PTCount,
          ResultURL,
          SubmissionID,
          CustomerIdentifier_MerchantCode,
          CustomerIdentifier_LocationCode,
          CustomerIdentifier_CustomerCode,
          TerminalIdentifier_LocationCode,
          TerminalIdentifier_TerminalCode,
          TerminalIdentifier_MerchantCode
        };

        dispatch({ type: SUBMIT_CARD + FETCH });

        axios({
          method: 'post',
          url: URL3DSi,
          headers: { 'Content-type': 'application/x-www-form-urlencoded' },
          data
        });

        redirectUser(RedirectURL, SubmissionID);
      }
    } catch (e) {
      dispatch({ type: SUBMIT_CARD + FAILURE });
    }
  };
};
