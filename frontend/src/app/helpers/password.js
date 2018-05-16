export default ({
  password,
  minLength,
  preferedLength,
  minNonAlphaNumChars,
  preferedNonAlphaNumChars,
  regularExpression,
  policyStrings,
  usePolicy
}) => {
  const { length } = password;

  // DEFINE MINIMUM THRESHOLD
  // required
  if (!length) return getResult(1);
  // minimum length
  if (minLength && length < minLength) return getResult(1);
  // minimum non-alpha symbols
  const numberNonAlphaNumChars = getNumberNonAlphaNumChars();
  if (minNonAlphaNumChars && numberNonAlphaNumChars < minNonAlphaNumChars) return getResult(1);
  // regexp
  if (regularExpression && !password.match(regularExpression)) return getResult(1);

  // DEFINE LEVELS
  const passwordStrength = getPasswordStrength();
  if (passwordStrength < 25) return getResult(1);
  if (passwordStrength < 50) return getResult(2);
  if (passwordStrength < 75) return getResult(3);
  if (passwordStrength < 100) return getResult(4);
  return getResult(5);


  function getResult(level) {
    let levelFormatted = level;
    if (usePolicy && level === 1) levelFormatted = 0;

    const valid = !!length && !!levelFormatted;

    return {
      level: levelFormatted,
      valid,
      message: policyStrings[length && levelFormatted]
    };
  }

  function getNumberNonAlphaNumChars() {
    const regExpList = password.match(/[^a-zA-Z\d\s:]/g);
    return regExpList ? regExpList.length : 0;
  }

  function getPasswordStrength() {
    const oneLengthPercent = preferedLength / 100.0;
    const lengthPercent = length / oneLengthPercent;

    const oneNonAlphaPercent = preferedNonAlphaNumChars / 100.0;
    const nonAplhaPercent = numberNonAlphaNumChars / oneNonAlphaPercent;
    const nonAplhaPercentFormatted = !isNaN(nonAplhaPercent) ? nonAplhaPercent : 100;

    const result = (lengthPercent + nonAplhaPercentFormatted) / 2;

    return result;
  }
};
