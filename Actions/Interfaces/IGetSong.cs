using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Actions
{
    interface IGetSong
    {
        Boolean Start(String url);
        Boolean Login(String loginName, String password);

        Boolean SearchSong();

        Boolean DownloadSong();

        Boolean ExtractSong();
    }
}
