using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalAPI.Application.Email
{
    public enum VerificationResult
    {
        Verifed = 0,
        Wrong = 1,
        Outdated = 2
    }
}
