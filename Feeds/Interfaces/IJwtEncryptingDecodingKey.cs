﻿using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Feeds
{
    public interface IJwtEncryptingDecodingKey
    {
        SecurityKey GetKey();
    }
}
