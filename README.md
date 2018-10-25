lindotnet
=========

.Net wrapper library for [liblinphone](http://www.linphone.org/eng/documentation/dev/liblinphone-free-sip-voip-sdk.html) library. You can use it for simple interface for SIP telephony.
Most of the code was borrowed from [sipdotnet](https://github.com/bedefaced/sipdotnet) .net wrapper.
But in the process of work it became necessary to expand the functionality. What led to the creation of this project.

On **10/26/2018** there is a beta release. The main functionality has been tested. Calls, messaging are performed.

*Current status: Testing. Development of functionality for video calls.*

Requirements
------------

* .NET 4.5.1 framework on Windows, Linux don't tested.
* included liblinphone [library binaries](https://github.com/FirsovMS/lindotnet/sdk/linphone.zip) version 4.0.1rc

### Important!

Do not try to use the latest release "linphone-sdk-4.1.1"!<br/>A problem with the audio output device has been noticed with it.<br/>Use the sdk supplied in the repository!

Documentation Page
------------------
Examples of usage can be found :

 * From the Unit tests `inside the repository`.
 * On the [page of my blog](https://firsovms.github.io/jekyll/update/2018/10/24/lindotnet-sdk-doc.html).
 * In my [softphone implementation](https://github.com/FirsovMS/MarmotVoipClient).

License
-------
[LGPLv3](http://en.wikipedia.org/wiki/GNU_Lesser_General_Public_License) (see `LICENSE` file)