module.exports = (callback, str) => {
	let min = require('luamin');
	
	callback(null, min.minify(str));
};