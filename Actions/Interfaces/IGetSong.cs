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
        Boolean SearchSong(String song, List<String> songNumbers, String downloadDirectory, String directoryName,String order,String waitToDownloadFile);
    }
}
